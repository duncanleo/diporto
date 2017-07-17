import * as React from 'react';
import { HTMLProps } from 'react';
import Nav from '../Nav';

export class Layout extends React.Component<{}, {}> {
    public render() {
	return (
	    <div>
		<Nav />
		<div>{this.props.children}</div>
	    </div>
	)
    }
}
