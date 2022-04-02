import { Action, Reducer } from 'redux';
import { AppThunkAction } from '.';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface TradeState {
    isLoading: boolean;
    trades: Trade[];
}

export interface Trade {
    id: string;
    timestamp: string;
    assetPairId: string;
    role: string;
    side: string;
    price: number;
    baseAssetId: string;
    baseVolume: string
    quoteAssetId: string;
    quoteVolume: string;
    fee: Fee;
}

export interface Fee {
    size: number;
    assetId: string;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface RequestTrades {
    type: 'REQUEST_TRADES';
}

interface ReceiveTrades {
    type: 'RECEIVE_TRADES';
    trades: Trade[];
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = RequestTrades | ReceiveTrades;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    requestTrades: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();
        if (appState && appState.trades) {
            fetch(`/Trader/api/trades`)
                .then(response => response.json() as Promise<Trade[]>)
                .then(data => {
                    dispatch({ type: 'RECEIVE_TRADES', trades: data });
                });
            dispatch({ type: 'REQUEST_TRADES' });
        }
    }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: TradeState = { trades: [], isLoading: false };

export const reducer: Reducer<TradeState> = (state: TradeState | undefined, incomingAction: Action): TradeState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'REQUEST_TRADES':
            return {
                trades: state.trades,
                isLoading: true,
            };
        case 'RECEIVE_TRADES':
            return {
                isLoading: false,
                trades: action.trades
            };
    }
    return state;
};
