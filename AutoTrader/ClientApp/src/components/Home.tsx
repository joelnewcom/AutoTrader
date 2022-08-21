import * as React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ApplicationState } from '../store';
import * as WalletStore from "../store/WalletStore";
import * as TraderStore from "../store/TradeStore"
import * as AssetPairStore from "../store/AssetPairStore"
import { Spinner, Button } from 'reactstrap';

type WalletProps =
  WalletStore.WalletState
  & TraderStore.TradeState // ... state we've requested from the Redux store
  & AssetPairStore.AssetPairState // ... state we've requested from the Redux store
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
    this.props.requestInfos();
  }

  private getWalletEntriesMutipliedByItsPrice() {
    var result: number[];
    result = [];

    this.props.balances.forEach((walletEntry, index) => {
      result[index] = 0
      result[index] = walletEntry.available * this.getPrice(this.getAssetPairId(walletEntry.assetId))
    })
    return result;
  }


  private getPrice(assetPairId: string): number {
    var price = this.props.prices.find(price => price.assetPairId === assetPairId)
    if (price !== undefined) {
      return price.ask
    }
    return 0;
  }

  private getAssetPairId(assetId: string): string {
    var assetPair = this.props.assetPairs.find(assetPair => assetPair.baseAssetId === assetId)
    if (assetPair !== undefined) {
      return assetPair.id
    }
    return ""
  }

  private renderOperations() {
    return (
      <div>
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
              this.getWalletEntriesMutipliedByItsPrice().reduce(function (reducer, obj) {
                return reducer += obj
              }, 0).toFixed(2)}
          </Button>
          {' '}
          <Button variant="primary" size="lg" disabled>
            backend version: {
              this.props.information.version}
          </Button>
        </div>
        <br></br>
        <div>
        {this.props.isLoadingOperations && <div> Operations loading <Spinner/></div>}
        {this.props.isLoadingBalances && <div> Balances loading <Spinner/></div>}
        {this.props.isLoadingPrices && <div> Prices loading <Spinner/></div>}
        {this.props.isLoadingAssetPairs && <div> AssetPairs loading <Spinner/></div>}
          
        </div>
      </div>
    );
  }

}

function mapStateToProps(state: ApplicationState) {
  return Object.assign({}, state.wallet, state.assetPairs);
}

function mapDispatchToProps(dispatch: any) {
  return bindActionCreators(Object.assign({}, WalletStore.actionCreators, AssetPairStore.actionCreators), dispatch);
}

export default connect(
  mapStateToProps, // Selects which state properties are merged into the component's props
  mapDispatchToProps // Selects which action creators are merged into the component's props
)(Home as any); // eslint-disable-line @typescript-eslint/no-explicit-any