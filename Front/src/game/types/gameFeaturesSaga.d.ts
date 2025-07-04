export interface CheckMapRequest {
    mapId?: number;
    mapSize: number;
    tilesPackName?: string;
}

export interface CheckMapResponse {
    direction: string;
    difficulty: string;
}
