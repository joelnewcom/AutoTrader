import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { ApplicationState } from '../store';
import * as WalletStore from "../store/WalletStore";
import { bindActionCreators } from 'redux';
import * as AssetPairStore from "../store/AssetPairStore"
import * as TraderStore from "../store/TradeStore"

// At runtime, Redux will merge together...
type WalletProps =
  WalletStore.WalletState // ... state we've requested from the Redux store
  & AssetPairStore.AssetPairState // ... state we've requested from the Redux store
  & TraderStore.TradeState // ... state we've requested from the Redux store
  & typeof WalletStore.actionCreators // ... plus action creators we've requested
  & RouteComponentProps; // ... plus incoming routing parameters


export interface Summary {
  assetId: string;
  available: number;
  reserved: number;
  availableInCHF: number;
  assetName: string;
  reservedInCHF: number;
}

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


  private enrichedWalletEntries() {
    var result: Summary[];
    result = [];

    this.props.balances.forEach((walletEntry, index) => {
      result[index] = { assetId: walletEntry.assetId, available: walletEntry.available, reserved: walletEntry.reserved, availableInCHF: 0, assetName: "", reservedInCHF: 0 }
      this.props.assetPairs.forEach(assetPair => {
        if (assetPair.baseAssetId == walletEntry.assetId) {
          this.props.prices.forEach(price => {
            if (price.assetPairId == assetPair.id) {
              result[index] = {
                assetId: walletEntry.assetId,
                available: walletEntry.available,
                reserved: walletEntry.reserved,
                availableInCHF: walletEntry.available * price.ask,
                assetName: assetPair.name,
                reservedInCHF: walletEntry.reserved * price.ask,
              }
            }
          })
        }
      })
    })
    return result;
  }

  private renderBalances() {
    return (
      <div>
        <h1>Balances</h1>
        <table className='table table-striped' aria-labelledby="tabelLabel">
          <thead>
            <tr>
              <th>assetId</th>
              <th>assetName</th>
              <th>Available</th>
              <th>Available in CHF</th>
              <th>Reserved</th>
              <th>Reserved in CHF</th>
            </tr>
          </thead>
          <tbody>
            {this.enrichedWalletEntries().map((balance: Summary) =>
              <tr key={balance.assetId}>
                <td>{balance.assetId}</td>
                <td>{balance.assetName}</td>
                <td>{balance.available}</td>
                <td>{balance.availableInCHF}</td>
                <td>{balance.reserved}</td>
                <td>{balance.reservedInCHF}</td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    );
  }

}

// export default connect(
//   (state: ApplicationState) => state.wallet, // Selects which state properties are merged into the component's props
//   WalletStore.actionCreators // Selects which action creators are merged into the component's props
// )(Wallet as any); // eslint-disable-line @typescript-eslint/no-explicit-any


function mapStateToProps(state: ApplicationState) {
  return Object.assign({}, state.wallet, state.assetPairs);
}

function mapDispatchToProps(dispatch: any) {
  return bindActionCreators(Object.assign({}, WalletStore.actionCreators, AssetPairStore.actionCreators), dispatch);
}

export default connect(
  mapStateToProps, // Selects which state properties are merged into the component's props
  mapDispatchToProps // Selects which action creators are merged into the component's props
)(Wallet as any); // eslint-disable-line @typescript-eslint/no-explicit-any