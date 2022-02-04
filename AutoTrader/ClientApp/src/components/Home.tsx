import * as React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ApplicationState } from '../store';
import * as WalletStore from "../store/WalletStore";
import * as TraderStore from "../store/TradeStore"
import * as AssetPairStore from "../store/AssetPairStore"
import { Button } from 'reactstrap';

type WalletProps =
  WalletStore.WalletState
  & TraderStore.TradeState // ... state we've requested from the Redux store
  & AssetPairStore.AutoTraderState // ... state we've requested from the Redux store
  & typeof TraderStore.actionCreators
  & typeof WalletStore.actionCreators // ... plus action creators we've requested
  & typeof AssetPairStore.actionCreators

export interface Summary {
  walletEntryAssetId: string;
  walletEntryAvailable: number;
  assetPair: string;
  price: number;
  valueInCHF: number;
}

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
        {this.renderOperations()}
      </React.Fragment>
    );
  }

  private ensureDataFetched() {
    this.props.requestOperations();
    this.props.requestBalances();
    this.props.requestPrices();
    this.props.requestAssetPairs();
  }

  private currentvalue() {
    var result: Summary[];
    result = [];

    this.props.balances.forEach((walletEntry, index) => {

      result[index] = { walletEntryAssetId: walletEntry.assetId, walletEntryAvailable: walletEntry.available, assetPair: "", price: 0, valueInCHF: walletEntry.available }
      this.props.assetPairs.forEach(assetPair => {
        if (assetPair.baseAssetId == walletEntry.assetId) {
          this.props.prices.forEach(price => {
            if (price.assetPairId == assetPair.id) {
              result[index] = {
                walletEntryAssetId: result[index].walletEntryAssetId,
                walletEntryAvailable: result[index].walletEntryAvailable,
                assetPair: assetPair.name,
                price: price.ask,
                valueInCHF: walletEntry.available * price.ask
              }
            }
          })
        }
      })
    })
    return result;
  }

  private renderOperations() {
    return (
      <div>
        <Button variant="primary" size="lg" disabled>
          Invested: {
            this.props.operations.reduce(function (reducer, obj) {
              return reducer += obj.totalVolume
            }, 0).toFixed(2)}
        </Button>
        {' '}
        <Button variant="primary" size="lg" disabled>
          Current value: {
            this.currentvalue().reduce(function (reducer, obj) {
              return reducer += obj.valueInCHF
            }, 0).toFixed(2)}
        </Button>

        {/* <table className='table table-striped' aria-labelledby="tabelLabel">
          <thead>
            <tr>
              <th>walletEntryAssetId</th>
              <th>walletEntryAvailable</th>
              <th>assetPair</th>
              <th>price</th>
              <th>valueInCHF</th>
            </tr>
          </thead>
          <tbody>
            {this.currentvalue().map((summary: Summary, index) =>
              <tr key={index}>
                <td>{summary.walletEntryAssetId}</td>
                <td>{summary.walletEntryAvailable}</td>
                <td>{summary.assetPair}</td>
                <td>{summary.price}</td>
                <td>{summary.valueInCHF}</td>
              </tr>
            )}
          </tbody>
        </table>

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
        </table> */}
      </div>
    );
  }

}


function mapStateToProps(state: ApplicationState) {
  return Object.assign({}, state.wallet, state.autoTrader);
}

function mapDispatchToProps(dispatch: any) {

  return bindActionCreators(Object.assign({}, WalletStore.actionCreators, AssetPairStore.actionCreators), dispatch);
  // return {
  //   actions: {
  //     todoActions: bindActionCreators(WalletStore.actionCreators, dispatch),
  //     counterActions: bindActionCreators(AssetPairStore.actionCreators, dispatch)
  //   }
  // };
}

export default connect(
  mapStateToProps, // Selects which state properties are merged into the component's props
  mapDispatchToProps // Selects which action creators are merged into the component's props
)(Home as any); // eslint-disable-line @typescript-eslint/no-explicit-any