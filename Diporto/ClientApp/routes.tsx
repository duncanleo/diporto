import * as React from 'react';
import { Route } from 'react-router-dom';
import { Layout } from './components/Layout';
import Home from './components/Home';
import PlacesSearch from './components/PlacesSearch';

export const routes = (
    <Layout>
	<Route exact path='/' component={ Home } />
	<Route path='/search/:searchTerm?' component={ PlacesSearch } />
    </Layout>
);
