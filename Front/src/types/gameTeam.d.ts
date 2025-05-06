interface TeamState {
    id: number;
    isCurrentUser?: boolean;
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
