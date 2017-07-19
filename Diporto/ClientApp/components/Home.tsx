import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { ChangeTargetHTMLProps } from 'react';
import { RouteComponentProps } from 'react-router-dom';
import Map from './Map';
import SearchBar from './SearchBar';

export default class Home extends React.Component<RouteComponentProps<{}>, {}> {
    public render() {
	return (
	    <div className="flex flex-column h-100 justify-center" style={{flexGrow:1}}>
		<div className="flex flex-column items-center">
		    <h1 className="white f1 lh-title">Diporto</h1>
		    <div className="flex w-100 mw7">
			<SearchBar
			    onSearch={(t) => console.log(t)}
			/>
		    </div>
		</div>
	    </div>
	);
    }
}
