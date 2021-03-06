import * as WeatherForecasts from './WeatherForecasts';
import * as Counter from './Counter';
import * as AssetPair from './AssetPairStore';
import * as WalletStore from './WalletStore';
import * as TradeStore from './TradeStore';
import * as ExceptionLogStore from './ExceptionLogStore';

// The top-level state object
export interface ApplicationState {
    counter: Counter.CounterState | undefined;
    weatherForecasts: WeatherForecasts.WeatherForecastsState | undefined;
    assetPairs: AssetPair.AssetPairState | undefined;
    wallet: WalletStore.WalletState | undefined;
    trades: TradeStore.TradeState | undefined;
    exceptionlogs: ExceptionLogStore.ExceptionLogState | undefined;
}

// Whenever an action is dispatched, Redux will update each top-level application state property using
// the reducer with the matching name. It's important that the names match exactly, and that the reducer
// acts on the corresponding ApplicationState property type.
export const reducers = {
    counter: Counter.reducer,
    weatherForecasts: WeatherForecasts.reducer,
    assetPairs: AssetPair.reducer,
    wallet: WalletStore.reducer,
    trades: TradeStore.reducer,
    exceptionlogs: ExceptionLogStore.reducer
};

// This type can be used as a hint on action creators so that its 'dispatch' and 'getState' params are
// correctly typed to match your store.
export interface AppThunkAction<TAction> {
    (dispatch: (action: TAction) => void, getState: () => ApplicationState): void;
}
