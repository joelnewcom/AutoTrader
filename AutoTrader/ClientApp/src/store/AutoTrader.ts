import { Action, Reducer } from 'redux';
import { AppThunkAction } from '.';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface AutoTraderState {
    isLoading: boolean;
    trades: AutoTraderData;
    assetPairHistoryEntries: AutoTraderIAssetPairHistoryEntry[];
}

export interface AutoTraderData {
    id: string;
    name: string;
    accuracy: number;
}

 export interface AutoTraderIAssetPairHistoryEntry{
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

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = RequestAutoTraderDataAction | ReceiveAutoTraderDataAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    requestAutoTraderData: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();
        if (appState && appState.autoTrader) {
            fetch(`trader`)
                .then(response => response.json() as Promise<AutoTraderIAssetPairHistoryEntry[]>)
                .then(data => {
                    dispatch({ type: 'RECEIVE_AUTOTRADER_DATA', assetPairHistoryEntries: data });
                });

            //dispatch({ type: 'REQUEST_AUTOTRADER_DATA'});
        }
    }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: AutoTraderState = {trades: { id:"no", name: "no", accuracy: 0}, isLoading: false, assetPairHistoryEntries:[]};

export const reducer: Reducer<AutoTraderState> = (state: AutoTraderState | undefined, incomingAction: Action): AutoTraderState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'REQUEST_AUTOTRADER_DATA':
            return {
                trades: state.trades,
                assetPairHistoryEntries: state.assetPairHistoryEntries,
                isLoading: true
            };
        case 'RECEIVE_AUTOTRADER_DATA':
            // Only accept the incoming data if it matches the most recent request. This ensures we correctly
            // handle out-of-order responses.
            return {
                isLoading: false,
                trades: {id:"", accuracy:0, name:""},
                assetPairHistoryEntries: action.assetPairHistoryEntries
            };
    }

    return state;
};
