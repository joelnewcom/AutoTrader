import { Action, Reducer } from 'redux';
import { AppThunkAction } from '.';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface WalletState {
    isLoading: boolean;
    balances: Balance[];
}

export interface Balance {
    assetId: string;
    available: number;
    reserved: number;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface RequestBalances {
    type: 'REQUEST_BALANCES';
}

interface ReceiveBalances {
    type: 'RECEIVE_BALANCES';
    balances: Balance[];
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = RequestBalances | ReceiveBalances;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    requestBalances: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();
        console.log("Call Balance");
        if (appState && appState.wallet) {
            console.log("Call Balance");
            fetch(`/Wallet/api/balance`)
                .then(response => response.json() as Promise<Balance[]>)
                .then(data => {
                    dispatch({ type: 'RECEIVE_BALANCES', balances: data });
                });
            dispatch({ type: 'REQUEST_BALANCES' });
        }
    }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: WalletState = { balances: [], isLoading: false };

export const reducer: Reducer<WalletState> = (state: WalletState | undefined, incomingAction: Action): WalletState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'REQUEST_BALANCES':
            return {
                balances: state.balances,
                isLoading: true,
            };
        case 'RECEIVE_BALANCES':
            return {
                isLoading: false,
                balances: action.balances
            };
    }
    return state;
};
