import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { ApplicationState } from '../store';
import * as WalletStore from "../store/WalletStore";

type WalletProps =
  WalletStore.WalletState // ... state we've requested from the Redux store
  & typeof WalletStore.actionCreators // ... plus action creators we've requested
  & RouteComponentProps; // ... plus incoming routing parameters

class Home extends React.PureComponent<WalletProps> {
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
        <h1 id="tabelLabel">Hello World</h1>
        {this.renderOperations()}
      </React.Fragment>
    );
  }

  private ensureDataFetched() {
    this.props.requestOperations();
  }

  private renderOperations() {
    return (
      <div>
        <h1>Operations</h1>
        <table className='table table-striped' aria-labelledby="tabelLabel">
          <thead>
            <tr>
              <th>time</th>
              <th>assetId</th>
              <th>type</th>
              <th>volume</th>
            </tr>
          </thead>
          <tbody>
            {this.props.operations.map((operation: WalletStore.Operation) =>
              <tr key={operation.assetId}>
                <td>{operation.timestamp}</td>
                <td>{operation.assetId}</td>
                <td>{operation.type}</td>
                <td>{operation.totalVolume}</td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    );
  }

}

export default connect(
  (state: ApplicationState) => state.wallet, // Selects which state properties are merged into the component's props
  WalletStore.actionCreators // Selects which action creators are merged into the component's props
)(Home as any); // eslint-disable-line @typescript-eslint/no-explicit-any