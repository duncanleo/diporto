import * as React from 'react';
import { ChangeTargetHTMLProps } from 'react';
import { RouteComponentProps } from 'react-router-dom';
import Map from './Map';

export default class Home extends React.Component<RouteComponentProps<{}>, {}> {
    public render() {
	return (
	    <div>
		<h1>Diporto</h1>
		<div>
		    <Map
		       width={window.innerWidth}
		       height={window.innerHeight}
		       viewport={{latitude: 1.03, longitude: 103}}
		    />
		    <input className="f6 f5-l input-reset bn fl black-80 bg-white pa3 lh-solid w-100 w-75-m w-80-l br2-ns br--left-ns" placeholder="Search" type="text" name="search" value=""/>
		    <input className="f6 f5-l button-reset fl pv3 tc bn bg-animate bg-black-70 hover-bg-black white pointer w-100 w-25-m w-20-l br2-ns br--right-ns" type="submit" value="subscribe"/>
		</div>
	    </div>
	);
    }
}
