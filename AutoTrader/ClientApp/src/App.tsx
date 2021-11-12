import * as React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import Counter from './components/Counter';
import AssetPairs from './components/AssetPairs'
import Wallet from './components/Wallet';
import './custom.css'

export default () => (
    <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/counter' component={Counter} />
        <Route path='/assetpairs' component={AssetPairs} />
        <Route path='/balance' component={Wallet} />
    </Layout>
);
