import { Action, Reducer } from 'redux';
import { AppThunkAction } from '.';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface AssetPairState {
    isLoading: boolean;
    assetPairs: AssetPairs[];
    selectedAssetPair: string;
    assetPairHistoryEntries: AutoTraderIAssetPairHistoryEntry[];
    logBooks: LogBook[];
    information: BackendInfo;
}

export interface AssetPairs {
    id: string;
    name: string;
    priceAccuracy: number;
    baseAssetId: string;
    quotingAssetId: string;
    quoteAssetAccuracy: number;
    baseAssetAccuracy: number;
}

export interface AutoTraderIAssetPairHistoryEntry {
    date: string;
    ask: number;
    bid: number;
}

export interface LogBook {
    assetPairId: string,
    date: string,
    logBookEntry: string,
    reason: string
}

export interface BackendInfo {
    version: string
}
// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface RequestAssetPairHistoryDataAction {
    type: 'REQUEST_ASSETPAIR_HISTORY_DATA';
    selectedAssetPair: string;
}

interface ReceiveAssetPairHistoryDataAction {
    type: 'RECEIVE_ASSETPAIR_HISTORY_DATA';
    assetPairHistoryEntries: AutoTraderIAssetPairHistoryEntry[];
}

interface RequestAssetPairLogBookDataAction {
    type: 'REQUEST_ASSETPAIR_LOGBOOK_DATA';
    selectedAssetPair: string;
}

interface ReceiveAssetPairLogBookDataAction {
    type: 'RECEIVE_ASSETPAIR_LOGBOOK_DATA';
    logBooks: LogBook[];
}

interface RequestAssetPairAction {
    type: 'REQUEST_ASSETPAIRS';
}

interface ReceiveAssetPairs {
    type: 'RECEIVE_ASSET_PAIRS';
    assetPairs: AssetPairs[];
}

interface RequestInfoAction {
    type: 'REQUEST_INFO';
}

interface ReceiveInfoAction {
    type: 'RECEIVE_INFO';
    information: BackendInfo;
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = RequestAssetPairHistoryDataAction | ReceiveAssetPairHistoryDataAction | ReceiveAssetPairs | RequestAssetPairAction | RequestAssetPairLogBookDataAction | ReceiveAssetPairLogBookDataAction | RequestInfoAction | ReceiveInfoAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {

    requestHistoryEntries: (assetPair: string): AppThunkAction<KnownAction> => (dispatch, getState) => {
        const appState = getState();
        if (appState && appState.assetPairs) {
            console.log("Call AssetPairHistoryEntries");
            fetch(`/Trader/api/AssetPairHistoryEntries/` + assetPair)
                .then(response => response.json() as Promise<AutoTraderIAssetPairHistoryEntry[]>)
                .then(data => {
                    dispatch({ type: 'RECEIVE_ASSETPAIR_HISTORY_DATA', assetPairHistoryEntries: data });
                });
            dispatch({ type: 'REQUEST_ASSETPAIR_HISTORY_DATA', selectedAssetPair: assetPair });
        }
    },

    requestAssetPairs: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
        const appState = getState();
        if (appState && appState.assetPairs) {
            console.log("Call GET AssetPairs");
            fetch(`/Trader/api/AssetPairs`)
                .then(response => response.json() as Promise<AssetPairs[]>)
                .then(data => {
                    dispatch({ type: 'RECEIVE_ASSET_PAIRS', assetPairs: data });
                });
            dispatch({ type: 'REQUEST_ASSETPAIRS' });
        }
    },

    requestLogBook: (assetPair: string): AppThunkAction<KnownAction> => (dispatch, getState) => {
        const appState = getState();
        if (appState && appState.assetPairs) {
            console.log("Call Logbook");
            fetch(`/Trader/api/Logbooks/` + assetPair)
                .then(response => response.json() as Promise<LogBook[]>)
                .then(data => {
                    dispatch({ type: 'RECEIVE_ASSETPAIR_LOGBOOK_DATA', logBooks: data });
                });
            dispatch({ type: 'REQUEST_ASSETPAIR_LOGBOOK_DATA', selectedAssetPair: assetPair });
        }
    },

    requestInfos: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
        const appState = getState();
        if (appState && appState.assetPairs) {
            fetch(`/Trader/api/info/`)
                .then(response => response.json() as Promise<BackendInfo>)
                .then(data => {
                    dispatch({ type: 'RECEIVE_INFO', information: data });
                });
            dispatch({ type: 'REQUEST_INFO' });
        }
    },
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: AssetPairState = { assetPairs: [], isLoading: false, assetPairHistoryEntries: [], selectedAssetPair: "", logBooks: [], information: { version: "n/a" } };

export const reducer: Reducer<AssetPairState> = (state: AssetPairState | undefined, incomingAction: Action): AssetPairState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'REQUEST_ASSETPAIR_HISTORY_DATA':
            return {
                assetPairs: state.assetPairs,
                assetPairHistoryEntries: state.assetPairHistoryEntries,
                isLoading: true,
                selectedAssetPair: action.selectedAssetPair,
                logBooks: state.logBooks,
                information: state.information
            };
        case 'RECEIVE_ASSETPAIR_HISTORY_DATA':
            return {
                isLoading: false,
                assetPairs: state.assetPairs,
                assetPairHistoryEntries: action.assetPairHistoryEntries,
                selectedAssetPair: state.selectedAssetPair,
                logBooks: state.logBooks,
                information: state.information
            };
        case 'REQUEST_ASSETPAIR_LOGBOOK_DATA':
            return {
                assetPairs: state.assetPairs,
                assetPairHistoryEntries: state.assetPairHistoryEntries,
                isLoading: true,
                selectedAssetPair: action.selectedAssetPair,
                logBooks: state.logBooks,
                information: state.information
            };
        case 'RECEIVE_ASSETPAIR_LOGBOOK_DATA':
            return {
                isLoading: false,
                assetPairs: state.assetPairs,
                assetPairHistoryEntries: state.assetPairHistoryEntries,
                selectedAssetPair: state.selectedAssetPair,
                logBooks: action.logBooks,
                information: state.information
            };
        case 'REQUEST_ASSETPAIRS':
            return {
                isLoading: true,
                assetPairs: state.assetPairs,
                assetPairHistoryEntries: state.assetPairHistoryEntries,
                selectedAssetPair: state.selectedAssetPair,
                logBooks: state.logBooks,
                information: state.information
            }
        case 'RECEIVE_ASSET_PAIRS':
            return {
                isLoading: false,
                assetPairs: action.assetPairs,
                assetPairHistoryEntries: state.assetPairHistoryEntries,
                selectedAssetPair: state.selectedAssetPair,
                logBooks: state.logBooks,
                information: state.information
            };
        case 'REQUEST_INFO':
            return {
                isLoading: true,
                assetPairs: state.assetPairs,
                assetPairHistoryEntries: state.assetPairHistoryEntries,
                selectedAssetPair: state.selectedAssetPair,
                logBooks: state.logBooks,
                information: state.information
            };
        case 'RECEIVE_INFO':
            return {
                isLoading: false,
                assetPairs: state.assetPairs,
                assetPairHistoryEntries: state.assetPairHistoryEntries,
                selectedAssetPair: state.selectedAssetPair,
                logBooks: state.logBooks,
                information: action.information
            };
    }

    return state;
};
