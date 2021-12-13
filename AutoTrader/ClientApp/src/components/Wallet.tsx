import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { ApplicationState } from '../store';
import * as WalletStore from "../store/WalletStore";

// At runtime, Redux will merge together...
type WalletProps =
  WalletStore.WalletState // ... state we've requested from the Redux store
  & typeof WalletStore.actionCreators // ... plus action creators we've requested
  & RouteComponentProps; // ... plus incoming routing parameters


class Wallet extends React.PureComponent<WalletProps> {
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
        {this.renderBalances()}
      </React.Fragment>
    );
  }

  private ensureDataFetched() {
    this.props.requestBalances();
  }

  private renderBalances() {
    return (
      <div>
        <h1>AssetPairs</h1>
        <table className='table table-striped' aria-labelledby="tabelLabel">
          <thead>
            <tr>
              <th>assetId</th>
              <th>Available</th>
              <th>Reserved</th>
            </tr>
          </thead>
          <tbody>
            {this.props.balances.map((balance: WalletStore.Balance) =>
              <tr key={balance.assetId}>
                <td>{balance.assetId}</td>
                <td>{balance.available}</td>
                <td>{balance.reserved}</td>
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
)(Wallet as any); // eslint-disable-line @typescript-eslint/no-explicit-any
