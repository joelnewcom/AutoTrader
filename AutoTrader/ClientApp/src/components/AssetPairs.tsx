import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { ApplicationState } from '../store';
import * as AutoTradersStore from "../store/AssetPairStore";

// At runtime, Redux will merge together...
type AutoTraderProps =
  AutoTradersStore.AutoTraderState // ... state we've requested from the Redux store
  & typeof AutoTradersStore.actionCreators // ... plus action creators we've requested
  & RouteComponentProps<{ startDateIndex: string }>; // ... plus incoming routing parameters


class AssetPairs extends React.PureComponent<AutoTraderProps> {
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
        {this.renderAssetPairHistoryPairTable()}
      </React.Fragment>
    );
  }

  private ensureDataFetched() {
    this.props.requestAutoTraderData();
    this.props.requestAssetPairs();
  }

  private renderAssetPairHistoryPairTable() {
    return (
      <div>
        <h1>AssetPairs</h1>
        <table className='table table-striped' aria-labelledby="tabelLabel">
          <thead>
            <tr>
              <th>Id</th>
              <th>Name</th>
              <th>Accuracy</th>
            </tr>
          </thead>
          <tbody>
            {this.props.assetPairs.map((assetPairs: AutoTradersStore.AssetPairs) =>
              <tr key={assetPairs.id}>
                <td>
                  <button type="button"
                    className="btn btn-primary btn-lg"
                    onClick={() => { this.props.requestHistoryEntries(assetPairs.id); }}>
                    {assetPairs.id}
                  </button>
                </td>
                <td>{assetPairs.name}</td>
                <td>{assetPairs.accuracy}</td>
              </tr>
            )}
          </tbody>
        </table>
        <h1>History entries of {this.props.selectedAssetPair}</h1>
        <table className='table table-striped' aria-labelledby="tabelLabel">
          <thead>
            <tr>
              <th>Date</th>
              <th>Ask</th>
              <th>Buy</th>
            </tr>
          </thead>
          <tbody>
            {this.props.assetPairHistoryEntries.map((assetPairHistoryEntries: AutoTradersStore.AutoTraderIAssetPairHistoryEntry) =>
              <tr key={assetPairHistoryEntries.date}>
                <td>{assetPairHistoryEntries.date}</td>
                <td>{assetPairHistoryEntries.ask}</td>
                <td>{assetPairHistoryEntries.buy}</td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    );
  }

}

export default connect(
  (state: ApplicationState) => state.autoTrader, // Selects which state properties are merged into the component's props
  AutoTradersStore.actionCreators // Selects which action creators are merged into the component's props
)(AssetPairs as any); // eslint-disable-line @typescript-eslint/no-explicit-any
