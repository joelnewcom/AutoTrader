import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { ApplicationState } from '../store';
import * as TradeStore from "../store/TradeStore";

// At runtime, Redux will merge together...
type TradesProps =
  TradeStore.TradeState // ... state we've requested from the Redux store
  & typeof TradeStore.actionCreators // ... plus action creators we've requested
  & RouteComponentProps; // ... plus incoming routing parameters


class Trades extends React.PureComponent<TradesProps> {
  // This method is called when the component is first added to the document
  public componentDidMount() {
    this.ensureDataFetched();
  }

  // This method is called when the route parameters change
  public componentDidUpdate() {
  }

  public render() {
    return (
      <React.Fragment>
        <h1 id="tabelLabel">Lykke Autotrader</h1>
        <p>Displaying live data from autoreader</p>
        {this.renderTrades()}
      </React.Fragment>
    );
  }

  private ensureDataFetched() {
    this.props.requestTrades();
  }

  private renderTrades() {
    return (
      <div>
        <h1>Trades</h1>
        <table className='table table-striped' aria-labelledby="tabelLabel">
          <thead>
            <tr>
              <th>Id</th>
              <th>timestamp</th>
              <th>assetPairId</th>
              <th>role</th>
              <th>side</th>
              <th>price</th>
              <th>baseAssetId</th>
              <th>baseVolume</th>
              <th>quoteAssetId</th>
              <th>quoteVolume</th>
              <th>fee</th>
            </tr>
          </thead>
          <tbody>
            {this.props.trades.map((trade: TradeStore.Trade) =>
              <tr key={trade.id}>
                <td>{trade.id}</td>
                <td>{trade.timestamp}</td>
                <td>{trade.assetPairId}</td>
                <td>{trade.role}</td>
                <td>{trade.side}</td>
                <td>{trade.price}</td>
                <td>{trade.baseAssetId}</td>
                <td>{trade.baseVolume}</td>
                <td>{trade.quoteAssetId}</td>
                <td>{trade.quoteVolume}</td>
                <td>AssetId: {trade.fee.assetId} ; Size: {trade.fee.size} </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    );
  }

}

export default connect(
  (state: ApplicationState) => state.trades, // Selects which state properties are merged into the component's props
  TradeStore.actionCreators // Selects which action creators are merged into the component's props
)(Trades as any); // eslint-disable-line @typescript-eslint/no-explicit-any
