import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { Link } from 'react-router-dom';
import { ApplicationState } from '../store';
import * as AutoTradersStore from "../store/AutoTrader";

// At runtime, Redux will merge together...
type AutoTraderProps =
  AutoTradersStore.AutoTraderState // ... state we've requested from the Redux store
  & typeof AutoTradersStore.actionCreators // ... plus action creators we've requested
  & RouteComponentProps<{ startDateIndex: string }>; // ... plus incoming routing parameters


class AutoTraderSite extends React.PureComponent<AutoTraderProps> {
  // This method is called when the component is first added to the document
  public componentDidMount() {
    this.ensureDataFetched();
  }

  // This method is called when the route parameters change
  public componentDidUpdate() {
    this.ensureDataFetched();
  }

  public render() {
    return (
      <React.Fragment>
        <h1 id="tabelLabel">Weather forecast</h1>
        <p>This component demonstrates fetching data from the server and working with URL parameters.</p>
        {this.renderForecastsTable()}
      </React.Fragment>
    );
  }

  private ensureDataFetched() {
    this.props.requestAutoTraderData();
  }

  private renderForecastsTable() {
      return (
          <div>
              <div>{this.props.trades.id}</div>
              <div>{this.props.trades.name}</div>
              <div>{this.props.trades.accuracy}</div>
          </div>
                    
    );
  }

}

export default connect(
  (state: ApplicationState) => state.autoTrader, // Selects which state properties are merged into the component's props
  AutoTradersStore.actionCreators // Selects which action creators are merged into the component's props
)(AutoTraderSite as any); // eslint-disable-line @typescript-eslint/no-explicit-any
