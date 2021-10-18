import { Action, Reducer } from 'redux';
import { AppThunkAction } from '.';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface AutoTraderState {
    isLoading: boolean;
    assetPairs: AssetPairs[];
    assetPairHistoryEntries: AutoTraderIAssetPairHistoryEntry[];
}

export interface AssetPairs {
    id: string;
    name: string;
    accuracy: number;
}

export interface AutoTraderIAssetPairHistoryEntry {
    date: string;
    ask: number;
    buy: number;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface RequestAutoTraderDataAction {
    type: 'REQUEST_AUTOTRADER_DATA';
}


interface ReceiveAutoTraderDataAction {
    type: 'RECEIVE_AUTOTRADER_DATA';
    assetPairHistoryEntries: AutoTraderIAssetPairHistoryEntry[];
}

interface RequestAssetPairAction {
    type: 'REQUEST_ASSETPAIRS';
}

interface ReceiveAssetPairs {
    type: 'RECEIVE_ASSET_PAIRS';
    assetPairs: AssetPairs[];
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = RequestAutoTraderDataAction | ReceiveAutoTraderDataAction | ReceiveAssetPairs | RequestAssetPairAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    requestAutoTraderData: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();
        if (appState && appState.autoTrader) {
            console.log("Call AssetPairHistoryEntries");
            fetch(`/Trader/api/AssetPairHistoryEntries`)
                .then(response => response.json() as Promise<AutoTraderIAssetPairHistoryEntry[]>)
                .then(data => {
                    dispatch({ type: 'RECEIVE_AUTOTRADER_DATA', assetPairHistoryEntries: data });
                });
            dispatch({ type: 'REQUEST_AUTOTRADER_DATA' });
        }
    },

    requestAssetPairs: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();
        if (appState && appState.autoTrader) {
            console.log("Call GET AssetPairs");
            fetch(`/Trader/api/AssetPairs`)
                .then(response => response.json() as Promise<AssetPairs[]>)
                .then(data => {
                    dispatch({ type: 'RECEIVE_ASSET_PAIRS', assetPairs: data });
                });
            dispatch({ type: 'REQUEST_ASSETPAIRS' });
        }
    }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: AutoTraderState = { assetPairs: [], isLoading: false, assetPairHistoryEntries: [] };

export const reducer: Reducer<AutoTraderState> = (state: AutoTraderState | undefined, incomingAction: Action): AutoTraderState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'REQUEST_AUTOTRADER_DATA':
            return {
                assetPairs: state.assetPairs,
                assetPairHistoryEntries: state.assetPairHistoryEntries,
                isLoading: true
            };
        case 'RECEIVE_AUTOTRADER_DATA':
            return {
                isLoading: false,
                assetPairs: state.assetPairs,
                assetPairHistoryEntries: action.assetPairHistoryEntries
            };
        case 'REQUEST_ASSETPAIRS':
            return {
                isLoading: true,
                assetPairs: state.assetPairs,
                assetPairHistoryEntries: state.assetPairHistoryEntries
            }
        case 'RECEIVE_ASSET_PAIRS':
            return {
                isLoading: false,
                assetPairs: action.assetPairs,
                assetPairHistoryEntries: state.assetPairHistoryEntries
            };
    }

    return state;
};
