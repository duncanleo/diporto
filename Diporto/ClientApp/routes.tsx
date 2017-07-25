import * as React from 'react';
import { Route } from 'react-router-dom';
import Layout  from './components/Layout';
import Home from './components/Home';
import PlacesSearch from './components/PlacesSearch';
import PlaceDisplay from './components/PlaceDisplay';
import Login from './components/Login';
import SignUp from './components/SignUp';

export const routes = (
    <Layout>
		<Route exact path='/' component={ Home } />
		<Route path="/login" component={ Login }/>
		<Route path="/signup" component={ SignUp }/>
		<Route path='/search/:searchTerm?' component={ PlacesSearch } />
		<Route path='/place/:id' component={ PlaceDisplay } />
    </Layout>
);
