import * as React from 'react';
import { Route } from 'react-router-dom';
import { Layout, LandingLayout, AppLayout } from './components/Layout';
import Home from './components/Home';

export const routes = (
    <Layout>
	<LandingLayout>
	    <Route exact path='/' component={ Home } />
	</LandingLayout>
    </Layout>
);
