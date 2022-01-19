import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { ApplicationState } from '../store';
import * as AssetPairStore from "../store/AssetPairStore";
import { ResponsiveContainer, LineChart, Line, XAxis, YAxis, CartesianGrid, Legend, Tooltip } from 'recharts';
import moment from 'moment';
import { Button } from 'reactstrap';

// At runtime, Redux will merge together...
type AutoTraderProps =
  AssetPairStore.AutoTraderState // ... state we've requested from the Redux store
  & typeof AssetPairStore.actionCreators // ... plus action creators we've requested
  & RouteComponentProps<{ startDateIndex: string }>; // ... plus incoming routing parameters


class AssetPairs extends React.PureComponent<AutoTraderProps> {
  // This method is called when the component is first added to the document
  public componentDidMount() {
    const assetPairs = this.props.requestAssetPairs();
  }

  // This method is called when the route parameters change
  public componentDidUpdate() {
  }

  public render() {
    return (
      <React.Fragment>
        {this.renderPage()}
      </React.Fragment>
    );
  }

  private renderPage() {
    return (
      <div>
        <h1>AssetPairs</h1>
        {this.props.assetPairs.map((assetPairs: AssetPairStore.AssetPairs) =>
          <Button className="assetPairButton" color="primary" key={assetPairs.id} outline size="sm" onClick={() => {
            this.props.requestHistoryEntries(assetPairs.id);
            this.props.requestLogBook(assetPairs.id);
          }}>
            {assetPairs.id}
          </Button>
        )}

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

        <h1>Logentries of {this.props.selectedAssetPair}</h1>
        <table className='table table-striped' aria-labelledby="tabelLabel">
          <thead>
            <tr>
              <th>Date</th>
              <th>Reason</th>
              <th>Entry</th>
            </tr>
          </thead>
          <tbody>
            {this.props.logBooks.sort((a, b) => new Date(b.date).getTime() - new Date(a.date).getTime()).map((logBook: AssetPairStore.LogBook, index) =>
              <tr key={index} >
                <td>{this.formatDateString(logBook.date)}</td>
                <td>{logBook.reason}</td>
                <td>{logBook.logBookEntry}</td>
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
            {this.props.assetPairHistoryEntries.map((assetPairHistoryEntries: AssetPairStore.AutoTraderIAssetPairHistoryEntry, index) =>
              <tr key={index} >
                <td>{this.formatDateString(assetPairHistoryEntries.date)}</td>
                <td>{assetPairHistoryEntries.ask}</td>
                <td>{assetPairHistoryEntries.bid}</td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    );
  }

  private formatDateString(tickItem: string) {
    return moment(tickItem).format('DD-MM-YYYY')
  }
}


export default connect(
  (state: ApplicationState) => state.autoTrader, // Selects which state properties are merged into the component's props
  AssetPairStore.actionCreators // Selects which action creators are merged into the component's props
)(AssetPairs as any); // eslint-disable-line @typescript-eslint/no-explicit-any
