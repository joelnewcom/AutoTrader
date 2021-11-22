import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { ApplicationState } from '../store';
import * as AutoTradersStore from "../store/AssetPairStore";
import { ResponsiveContainer, LineChart, Line, XAxis, YAxis, CartesianGrid, Legend, Tooltip } from 'recharts';
import moment from 'moment';

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
              <th>Bid</th>
            </tr>
          </thead>
          <tbody>
            {this.props.assetPairHistoryEntries.map((assetPairHistoryEntries: AutoTradersStore.AutoTraderIAssetPairHistoryEntry) =>
              <tr key={this.formatDateString(assetPairHistoryEntries.date)}>
                <td>{this.formatDateString(assetPairHistoryEntries.date)}</td>
                <td>{assetPairHistoryEntries.ask}</td>
                <td>{assetPairHistoryEntries.bid}</td>
              </tr>
            )}
          </tbody>
        </table>
        <h1>Chart</h1>
        <ResponsiveContainer width="100%" height={400}>
          <LineChart
            data={this.props.assetPairHistoryEntries}
            margin={{
              top: 5,
              right: 5,
              left: 20,
              bottom: 25,
            }}
          >
            <Line type="monotone" dataKey="ask" stroke="#8884d8" />
            <Line type="monotone" dataKey="bid" stroke="#82ca9d" />
            <CartesianGrid stroke="#ccc" strokeDasharray="5 5" />
            <Legend />
            <Tooltip />
            <XAxis dataKey="date" tickFormatter={this.formatDateString} />
            <YAxis orientation="left" domain={["dataMin", "dataMax"]} />
          </LineChart>
        </ResponsiveContainer>
      </div>
    );
  }

  private formatDateString(tickItem: string) {
    return moment(tickItem).format('DD-MM-YYYY')
  }
}


export default connect(
  (state: ApplicationState) => state.autoTrader, // Selects which state properties are merged into the component's props
  AutoTradersStore.actionCreators // Selects which action creators are merged into the component's props
)(AssetPairs as any); // eslint-disable-line @typescript-eslint/no-explicit-any
