import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { NavLink, Link } from 'react-router-dom';
import { ChangeTargetHTMLProps } from 'react';
import { RouteComponentProps } from 'react-router-dom';
import Map from './Map';
import SearchBar from './SearchBar';
const config: Config = require('../util/config.json');

export default class Home extends React.Component<RouteComponentProps<{}>, {}> {
    public render() {
	const containerStyle = {
	    backgroundColor: "rgba(80, 204, 188, .8)",
	    flexGrow: 1
	};

	const backgroundStyle = {
	    backgroundImage: `url(https://api.mapbox.com/styles/v1/mapbox/streets-v9/static/103.9,1.29,11,.00,0.00/1000x600@2x?access_token=${config.MAPBOX_API_KEY})`,
	    flexGrow: 1
	}

	return (
	    <div style={backgroundStyle} className="flex flex-column">
		<div style={containerStyle} className="flex flex-column h-100 justify-center">
		    <div className="flex flex-column items-center pa4 pa0-ns">
			<h1 className="white f-subheadline lh-solid">Diporto</h1>
			<h2 className="white f4 lh-title tc">Best recommendations for the best places in Singapore.</h2>
			<div className="flex w-100 mw7">
			    <SearchBar
				onSearch={(t) => console.log(t)}
			    />
			</div>
			<Link className='white f5 mt4 lh-copy' to={ '/nearby' }>show me nearby places</Link>
		    </div>
		</div>
	    </div>
	);
    }
}
