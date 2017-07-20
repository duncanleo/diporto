import * as React from 'react';
import { HTMLProps } from 'react';
import Nav from '../Nav';

export class Layout extends React.Component<{}, {}> {
    public render() {
	return (
	    <div className="h-100 flex flex-column">
		<Nav />
		<div className="flex flex-column" style={{flexGrow: 1}}>
		    {this.props.children}
		</div>
	    </div>
	)
    }
}