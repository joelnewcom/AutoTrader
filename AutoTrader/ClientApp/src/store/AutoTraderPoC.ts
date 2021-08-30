import { Action, Reducer } from 'redux';
import { AppThunkAction } from '.';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface AutoTraderState {
    isLoading: boolean;
    startDateIndex?: number;
    forecasts: AutoTraderData[];
}

export interface AutoTraderData {
    date: string;
    temperatureC: number;
    temperatureF: number;
    summary: string;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface RequestAutoTraderDataAction {
    type: 'REQUEST_AUTOTRADER_DATA';
    startDateIndex: number;
}

interface ReceiveAutoTraderDataAction {
    type: 'RECEIVE_AUTOTRADER_DATA';
    startDateIndex: number;
    forecasts: AutoTraderData[];
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = RequestAutoTraderDataAction | ReceiveAutoTraderDataAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    requestAutoTraderData: (startDateIndex: number): AppThunkAction<KnownAction> => (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();
        if (appState && appState.weatherForecasts && startDateIndex !== appState.weatherForecasts.startDateIndex) {
            fetch(`weatherforecast`)
                .then(response => response.json() as Promise<AutoTraderData[]>)
                .then(data => {
                    dispatch({ type: 'RECEIVE_AUTOTRADER_DATA', startDateIndex: startDateIndex, forecasts: data });
                });

            dispatch({ type: 'REQUEST_AUTOTRADER_DATA', startDateIndex: startDateIndex });
        }
    }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: AutoTraderState = { forecasts: [], isLoading: false };

export const reducer: Reducer<AutoTraderState> = (state: AutoTraderState | undefined, incomingAction: Action): AutoTraderState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'REQUEST_AUTOTRADER_DATA':
            return {
                startDateIndex: action.startDateIndex,
                forecasts: state.forecasts,
                isLoading: true
            };
        case 'RECEIVE_AUTOTRADER_DATA':
            // Only accept the incoming data if it matches the most recent request. This ensures we correctly
            // handle out-of-order responses.
            if (action.startDateIndex === state.startDateIndex) {
                return {
                    startDateIndex: action.startDateIndex,
                    forecasts: action.forecasts,
                    isLoading: false
                };
            }
            break;
    }

    return state;
};
