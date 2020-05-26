using UnityEngine;
using System.IO;

public class HexCell : MonoBehaviour {

    // Coordinates of cell
	public HexCoordinates coordinates;

    // UI coordinates of cell
	public RectTransform uiRect;

    // Chunk that cell belongs to
	public HexGridChunk chunk;

    // Terrain type (indexes in Unity Editor)
    int terrainTypeIndex;

    // Elevation parameters
    int elevation = int.MinValue;

    // Water in cell with its lvel
    int waterLevel;

    // Special objects levels
    int urbanLevel, farmLevel, plantLevel;

    // Index of special object on the cell
    int specialIndex;

    // Walls on cell
    bool walled;

    // State of cell due to having water in cell or not
    bool hasIncomingRiver, hasOutgoingRiver;

    // Directions for incoming and outgoing rivers
    HexDirection incomingRiver, outgoingRiver;

    // Neighbors of cell (maximum 6 neighbors)
    [SerializeField]
    HexCell[] neighbors;

    // States for roads (maximum 6 roads in one cell)
    [SerializeField]
    bool[] roads;

    public int Elevation {
		get {
			return elevation;
		}
		set {
			if (elevation == value) {
				return;
			}
			elevation = value;
			RefreshPosition();
			ValidateRivers();

			for (int i = 0; i < roads.Length; i++) {
				if (roads[i] && GetElevationDifference((HexDirection)i) > 1) {
					SetRoad(i, false);
				}
			}

			Refresh();
		}
	}

	public int WaterLevel {
		get {
			return waterLevel;
		}
		set {
			if (waterLevel == value) {
				return;
			}
			waterLevel = value;
			ValidateRivers();
			Refresh();
		}
	}

	public bool IsUnderwater {
		get {
			return waterLevel > elevation;
		}
	}

	public bool HasIncomingRiver {
		get {
			return hasIncomingRiver;
		}
	}

	public bool HasOutgoingRiver {
		get {
			return hasOutgoingRiver;
		}
	}

	public bool HasRiver {
		get {
			return hasIncomingRiver || hasOutgoingRiver;
		}
	}

	public bool HasRiverBeginOrEnd {
		get {
			return hasIncomingRiver != hasOutgoingRiver;
		}
	}

	public HexDirection RiverBeginOrEndDirection {
		get {
			return hasIncomingRiver ? incomingRiver : outgoingRiver;
		}
	}

	public bool HasRoads {
		get {
			for (int i = 0; i < roads.Length; i++) {
				if (roads[i]) {
					return true;
				}
			}
			return false;
		}
	}

	public HexDirection IncomingRiver {
		get {
			return incomingRiver;
		}
	}

	public HexDirection OutgoingRiver {
		get {
			return outgoingRiver;
		}
	}

	public Vector3 Position {
		get {
			return transform.localPosition;
		}
	}


	public float StreamBedY {
		get {
			return
				(elevation + HexMetrics.streamBedElevationOffset) *
				HexMetrics.elevationStep;
		}
	}

	public float RiverSurfaceY {
		get {
			return
				(elevation + HexMetrics.waterElevationOffset) *
				HexMetrics.elevationStep;
		}
	}

	public float WaterSurfaceY {
		get {
			return
				(waterLevel + HexMetrics.waterElevationOffset) *
				HexMetrics.elevationStep;
		}
	}

	public int UrbanLevel {
		get {
			return urbanLevel;
		}
		set {
			if (urbanLevel != value) {
				urbanLevel = value;
				RefreshSelfOnly();
			}
		}
	}

	public int FarmLevel {
		get {
			return farmLevel;
		}
		set {
			if (farmLevel != value) {
				farmLevel = value;
				RefreshSelfOnly();
			}
		}
	}

	public int PlantLevel {
		get {
			return plantLevel;
		}
		set {
			if (plantLevel != value) {
				plantLevel = value;
				RefreshSelfOnly();
			}
		}
	}

	public int SpecialIndex {
		get {
			return specialIndex;
		}
		set {
			if (specialIndex != value && !HasRiver) {
				specialIndex = value;
				RemoveRoads();
				RefreshSelfOnly();
			}
		}
	}

	public bool IsSpecial {
		get {
			return specialIndex > 0;
		}
	}

	public bool Walled {
		get {
			return walled;
		}
		set {
			if (walled != value) {
				walled = value;
				Refresh();
			}
		}
	}

	public int TerrainTypeIndex {
		get {
			return terrainTypeIndex;
		}
		set {
			if (terrainTypeIndex != value) {
				terrainTypeIndex = value;
				Refresh();
			}
		}
	}

	public HexCell GetNeighbor (HexDirection direction) {
		return neighbors[(int)direction];
	}

	public void SetNeighbor (HexDirection direction, HexCell cell) {
		neighbors[(int)direction] = cell;
		cell.neighbors[(int)direction.Opposite()] = this;
	}

	public HexEdgeType GetEdgeType (HexDirection direction) {
		return HexMetrics.GetEdgeType(
			elevation, neighbors[(int)direction].elevation
		);
	}

	public HexEdgeType GetEdgeType (HexCell otherCell) {
		return HexMetrics.GetEdgeType(
			elevation, otherCell.elevation
		);
	}

	public bool HasRiverThroughEdge (HexDirection direction) {
		return
			hasIncomingRiver && incomingRiver == direction ||
			hasOutgoingRiver && outgoingRiver == direction;
	}

	public void RemoveIncomingRiver () {
		if (!hasIncomingRiver) {
			return;
		}

        // Set to 'false' status of incoming to cell river
		hasIncomingRiver = false;
		RefreshSelfOnly();

        // Find neighbor with incoming river direction
		HexCell neighbor = GetNeighbor(incomingRiver);
        // Delete incoming river for current cell
        // by selecting neighbor outgoingRivee = 'false' 
		neighbor.hasOutgoingRiver = false;
        // Refresh chunk for seeing changes
		neighbor.RefreshSelfOnly();
	}

	public void RemoveOutgoingRiver () {
		if (!hasOutgoingRiver) {
			return;
		}

        // Remove outgoing river from current cell
		hasOutgoingRiver = false;
		RefreshSelfOnly();

        // Get neighbor where river has going
		HexCell neighbor = GetNeighbor(outgoingRiver);
        // Set status of 'incoming' river of neighbor
        // to 'false'
        neighbor.hasIncomingRiver = false;
		neighbor.RefreshSelfOnly();
	}

	public void RemoveRiver () {
        // Remove all river types
        // which exist in current
        // cell
		RemoveOutgoingRiver();
		RemoveIncomingRiver();
	}

    // Set river flow to choosen direction
	public void SetOutgoingRiver (HexDirection direction) {
        // If we have river in choosen direction, just skip it
        if (hasOutgoingRiver && outgoingRiver == direction) {
			return;
		}

        // ............................................
        // REQUIREMENT 14.2 - main rule 1
        // ............................................
        // Check if cell have neigbor from selected direction
        // and: 
        //  current cell elevation >= neighbor cell from 
        //     selected directionelevation
        //    or
        // water level in current cell
        //   equals to neigbor cell elevation level
        HexCell neighbor = GetNeighbor(direction);
		if (!IsValidRiverDestination(neighbor)) {
			return;
		}

        // ............................................
        // REQUIREMENT 14.2 - 1.3
        // ............................................
        // remove old outgoing river to another direction
        RemoveOutgoingRiver();
        // ............................................

        // ............................................
        // REQUIREMENT 14.2 - main rule 2
        // REQUIREMENT 14.2 - 1.2.
        // ............................................
        //  If was incoming river in choosed direction,
        // remove incoming river
        if (hasIncomingRiver && incomingRiver == direction) {
			RemoveIncomingRiver();
		}
        // ............................................

        // ............................................
        // REQUIREMENT 14.2 - 1.1
        // ............................................
        // Set state of outgoing river
        // to selected direction
        // in cell to 'true'
        hasOutgoingRiver = true;
        // Set direction of outgoing river
		outgoingRiver = direction;
        // Set special index to 0
        // in cell
		specialIndex = 0;

        // Delete incoming river to current cell
		neighbor.RemoveIncomingRiver();

        // Set status of incoming river to cell
        // to 'true', because now river
        // incoming from current cell
        // to cell with opposite direction
		neighbor.hasIncomingRiver = true;
		neighbor.incomingRiver = direction.Opposite();
		neighbor.specialIndex = 0;
        // ............................................

        // REQUIREMENT 14.4 - 5
        // Delete road from selected direction
		SetRoad((int)direction, false);
	}

	public bool HasRoadThroughEdge (HexDirection direction) {
		return roads[(int)direction];
	}

    // Add road to selected direction
	public void AddRoad (HexDirection direction) {
        // If cell don't have road in selected direction
        // and no Special objects in cell
        // and neighbor cell in selected doesn't contain
        // Special object, and elevation dirrefence
        // between current cell and cell from selected direction
        // <= 1, set road to selected direction 
        
        // REQUIREMENT 14.3 - 1, 2, 3.2, 4
        if (
			!roads[(int)direction] && !HasRiverThroughEdge(direction) &&
			!IsSpecial && !GetNeighbor(direction).IsSpecial &&
			GetElevationDifference(direction) <= 1
		) {
			SetRoad((int)direction, true);
		}
	}

	public void RemoveRoads () {
        // For all roads in current cell:
        //  if road is exists, delete road
        // by setting road[i] = false
        for (int i = 0; i < neighbors.Length; i++) {
			if (roads[i]) {
				SetRoad(i, false);
			}
		}
	}

	public int GetElevationDifference (HexDirection direction) {
        // Get dirrerence between current cell elevation
        // and neigbor from selected direction
		int difference = elevation - GetNeighbor(direction).elevation;
        // If difference < 0, make it positive, else return
        // itself
        return difference >= 0 ? difference : -difference;
	}

	bool IsValidRiverDestination (HexCell neighbor) {
        // If current cell have selected neighbor
        // and one of following:
        // current cell elevation >= neighbor elevation
        //   or
        // current cell water level equals to neighbor elevation
        return neighbor && (
			elevation >= neighbor.elevation || waterLevel == neighbor.elevation
		);
	}

    // In that functions we decide what we need to do with
    // river changes: delete old because of new one
    //
	void ValidateRivers () {
        // If we have river that flow through current cell,
        // and don't have neighbor from that direction
        // or water level != current cell elevation
        // or neighbor cell elevation > current cell elevation,
        // we remove ougoing river
		if (
			hasOutgoingRiver &&
			!IsValidRiverDestination(GetNeighbor(outgoingRiver))
		) {
			RemoveOutgoingRiver();
		}
        // If we have river that flow into current cell,
        // and don't have neighbor from that direction
        // or water level != current cell elevation
        // or neighbor cell elevation > current cell elevation,
        // we remove incoming river
        if (
			hasIncomingRiver &&
			!GetNeighbor(incomingRiver).IsValidRiverDestination(this)
		) {
			RemoveIncomingRiver();
		}
	}

    // Update road function
	void SetRoad (int index, bool state) {
        // Set state of road
        // with selected index (0 - 5 : 6 roads)
        roads[index] = state;
        // Cast direction to index from enum
        // and get opposite direction to him
		neighbors[index].roads[(int)((HexDirection)index).Opposite()] = state;
        // Refresh neighbor cell in chunk
        neighbors[index].RefreshSelfOnly();
        // Refresh current cell in chunk
        RefreshSelfOnly();
	}

    // Update current position
    // of cell when update elevation
    void RefreshPosition () {
        // Get current cell position
		Vector3 position = transform.localPosition;
        // Update Y position by multiply current elevation
        // on elevation step
		position.y = elevation * HexMetrics.elevationStep;
        // add to Y position noise part
        // for perturbation
        position.y +=
			(HexMetrics.SampleNoise(position).y * 2f - 1f) *
			HexMetrics.elevationPerturbStrength;
		transform.localPosition = position;

        // Set new UI position
		Vector3 uiPosition = uiRect.localPosition;
		uiPosition.z = -position.y;
		uiRect.localPosition = uiPosition;
	}

    // Full update for neighbor
	void Refresh () {
        // If chunk exists
		if (chunk) {
            // Update chunk state
            chunk.Refresh();

            // Update chunks for each neighbor
            for (int i = 0; i < neighbors.Length; i++) {
				HexCell neighbor = neighbors[i];
				if (neighbor != null && neighbor.chunk != chunk) {
					neighbor.chunk.Refresh();
				}
			}
		}
	}

	void RefreshSelfOnly () {
        // Refresh chunk state
        chunk.Refresh();
	}

	public void Save (BinaryWriter writer) {
		writer.Write((byte)terrainTypeIndex);
		writer.Write((byte)elevation);
		writer.Write((byte)waterLevel);
		writer.Write((byte)urbanLevel);
		writer.Write((byte)farmLevel);
		writer.Write((byte)plantLevel);
		writer.Write((byte)specialIndex);
		writer.Write(walled);

		if (hasIncomingRiver) {
			writer.Write((byte)(incomingRiver + 128));
		}
		else {
			writer.Write((byte)0);
		}

		if (hasOutgoingRiver) {
			writer.Write((byte)(outgoingRiver + 128));
		}
		else {
			writer.Write((byte)0);
		}

		int roadFlags = 0;
		for (int i = 0; i < roads.Length; i++) {
			if (roads[i]) {
				roadFlags |= 1 << i;
			}
		}
		writer.Write((byte)roadFlags);
	}

	public void Load (BinaryReader reader) {
		terrainTypeIndex = reader.ReadByte();
		elevation = reader.ReadByte();
		RefreshPosition();
		waterLevel = reader.ReadByte();
		urbanLevel = reader.ReadByte();
		farmLevel = reader.ReadByte();
		plantLevel = reader.ReadByte();
		specialIndex = reader.ReadByte();
		walled = reader.ReadBoolean();

		byte riverData = reader.ReadByte();
		if (riverData >= 128) {
			hasIncomingRiver = true;
			incomingRiver = (HexDirection)(riverData - 128);
		}
		else {
			hasIncomingRiver = false;
		}

		riverData = reader.ReadByte();
		if (riverData >= 128) {
			hasOutgoingRiver = true;
			outgoingRiver = (HexDirection)(riverData - 128);
		}
		else {
			hasOutgoingRiver = false;
		}

		int roadFlags = reader.ReadByte();
		for (int i = 0; i < roads.Length; i++) {
			roads[i] = (roadFlags & (1 << i)) != 0;
		}
	}
}
