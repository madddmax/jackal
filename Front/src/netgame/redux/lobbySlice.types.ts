import { NetGameEntryResponse } from '/common/redux.types';

export interface LobbyGamesEntriesList {
    currentUserId?: number;
    gamesEntries: NetGameEntryResponse[];
}
