import { Action, Reducer } from 'redux';
import { AppThunkAction } from '.';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface WalletState {
    balances: Balance[];
    isLoadingBalances: boolean;
    operations: Operation[];
    isLoadingOperations: boolean;
    prices: Price[];
    isLoadingPrices: boolean;
}

export interface Balance {
    assetId: string;
    available: number;
    reserved: number;
}

export interface Operation {
    assetId: string;
    totalVolume: number;
    type: string;
    timestamp: string;
}

export interface Price {
    assetPairId: string;
    date: string;
    ask: number;
    bid: number;
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

interface RequestOperations {
    type: 'REQUEST_OPERATIONS';
}

interface ReceiveOperations {
    type: 'RECEIVE_OPERATIONS';
    operations: Operation[];
}

interface RequestPrices {
    type: 'REQUEST_PRICES';
}

interface ReceivePrices {
    type: 'RECEIVE_PRICES';
    prices: Price[];
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = RequestBalances | ReceiveBalances | RequestOperations | ReceiveOperations | RequestPrices | ReceivePrices;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    requestBalances: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
        const appState = getState();
        if (appState && appState.wallet) {
            fetch(`/Wallet/api/balance`)
                .then(response => response.json() as Promise<Balance[]>)
                .then(data => {
                    dispatch({ type: 'RECEIVE_BALANCES', balances: data });
                });
            dispatch({ type: 'REQUEST_BALANCES' });
        }
    },
    requestOperations: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
        const appState = getState();
        if (appState && appState.wallet) {
            fetch(`/Wallet/api/operations`)
                .then(response => response.json() as Promise<Operation[]>)
                .then(data => {
                    dispatch({ type: 'RECEIVE_OPERATIONS', operations: data });
                });
            dispatch({ type: 'REQUEST_OPERATIONS' });
        }
    },
    requestPrices: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
        const appState = getState();
        if (appState && appState.wallet) {
            fetch(`/Wallet/api/prices`)
                .then(response => response.json() as Promise<Price[]>)
                .then(data => {
                    dispatch({ type: 'RECEIVE_PRICES', prices: data });
                });
            dispatch({ type: 'REQUEST_PRICES' });
        }
    }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: WalletState = { balances: [], isLoadingBalances: false, operations: [], isLoadingOperations: false, prices: [], isLoadingPrices: false };

export const reducer: Reducer<WalletState> = (state: WalletState | undefined, incomingAction: Action): WalletState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'REQUEST_BALANCES':
            return {
                isLoadingBalances: true,
                isLoadingOperations: state.isLoadingOperations,
                isLoadingPrices: state.isLoadingPrices,
                balances: state.balances,
                operations: state.operations,
                prices: state.prices
            };
        case 'RECEIVE_BALANCES':
            return {
                isLoadingBalances: false,
                isLoadingOperations: state.isLoadingOperations,
                isLoadingPrices: state.isLoadingPrices,
                balances: action.balances,
                operations: state.operations,
                prices: state.prices
            };
        case 'REQUEST_OPERATIONS':
            return {
                isLoadingBalances: state.isLoadingBalances,
                isLoadingOperations: true,
                isLoadingPrices: state.isLoadingPrices,
                balances: state.balances,
                operations: state.operations,
                prices: state.prices
            };
        case 'RECEIVE_OPERATIONS':
            return {
                isLoadingBalances: state.isLoadingBalances,
                isLoadingOperations: false,
                isLoadingPrices: state.isLoadingPrices,
                balances: state.balances,
                operations: action.operations,
                prices: state.prices
            };
        case 'REQUEST_PRICES':
            return {
                isLoadingBalances: state.isLoadingBalances,
                isLoadingOperations: state.isLoadingOperations,
                isLoadingPrices: true,
                balances: state.balances,
                operations: state.operations,
                prices: state.prices
            };
        case 'RECEIVE_PRICES':
            return {
                isLoadingBalances: state.isLoadingBalances,
                isLoadingOperations: state.isLoadingOperations,
                isLoadingPrices: false,
                balances: state.balances,
                operations: state.operations,
                prices: action.prices
            }
    }
    return state;
};
