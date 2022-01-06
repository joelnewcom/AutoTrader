import { Action, Reducer } from 'redux';
import { AppThunkAction } from '.';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface ExceptionLogState {
    isLoading: boolean;
    exceptionLogs: ExceptionLog[];
}

export interface ExceptionLog {
    message: string;
    dateTime: Date;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface RequestExceptionLog {
    type: 'REQUEST_EXCEPTIONLOGS';
}

interface ReceiveExceptionLog {
    type: 'RECEIVE_EXCEPTIONLOGS';
    exceptionLogs: ExceptionLog[];
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = RequestExceptionLog | ReceiveExceptionLog;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    requestExceptionLogs: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();
        if (appState && appState.exceptionlogs) {
            fetch(`/ExceptionLog/api/excpetionlogs`)
                .then(response => response.json() as Promise<ExceptionLog[]>)
                .then(data => {
                    dispatch({ type: 'RECEIVE_EXCEPTIONLOGS', exceptionLogs: data });
                });
            dispatch({ type: 'REQUEST_EXCEPTIONLOGS' });
        }
    }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: ExceptionLogState = { exceptionLogs: [], isLoading: false };

export const reducer: Reducer<ExceptionLogState> = (state: ExceptionLogState | undefined, incomingAction: Action): ExceptionLogState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'REQUEST_EXCEPTIONLOGS':
            return {
                exceptionLogs: state.exceptionLogs,
                isLoading: true,
            };
        case 'RECEIVE_EXCEPTIONLOGS':
            return {
                isLoading: false,
                exceptionLogs: action.exceptionLogs
            };
        default:
            return state;
    }
};