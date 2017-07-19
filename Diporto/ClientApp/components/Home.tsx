import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { ChangeTargetHTMLProps } from 'react';
import { RouteComponentProps } from 'react-router-dom';
import Map from './Map';

export default class Home extends React.Component<RouteComponentProps<{}>, {}> {
    public render() {
	return (
	    <div className="flex flex-column h-100 justify-center" style={{flexGrow:1}}>
		<div className="flex flex-column items-center">
		    <h1 className="white f1 lh-title">Diporto</h1>
		    <div className="flex w-100 mw7">
			<input className="f6 f5-l input-reset bn fl black-80 bg-white pa3 lh-solid w-100 w-75-m w-80-l br2-ns br--left-n" placeholder="Search" type="text" name="search" value=""/>
			<input className="f6 f5-l button-reset fl pv3 tc bn bg-animate bg-black-70 hover-bg-black white pointer w-100 w-25-m w-20-l br2-ns br--right-ns" type="submit" value="search"/>
		    </div>
		</div>
	    </div>
	);
    }
}
