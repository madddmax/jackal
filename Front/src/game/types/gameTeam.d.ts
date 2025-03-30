export interface GameTeam {
    id: number;
    name: string;
    coins: number;
    isHuman: boolean;
    ship: {
        x: number;
        y: number;
    };
}

export interface TeamState {
    id: number;
    activePirate: string;
    name: string;
    backColor: string;
    group: TeamGroup;
    isHuman: boolean;
}

interface TeamGroup {
    id: string;
    photoMaxId: number;
    extension?: string;
}
