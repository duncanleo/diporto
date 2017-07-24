import * as React from 'react';
import { Link } from 'react-router-dom';
import { HTMLProps } from 'react';

const styles = {
  navBar: {
      backgroundColor: "#273CFE",
      flexShrink: 0,
  } as HTMLProps<HTMLDivElement>,
  buttonContainer: {
      alignSelf: "center"
  } as HTMLProps<HTMLDivElement>,
  loginButton: {
      color: "#FE92DE",
  },
  signUpButton: {
      backgroundColor: "#50CCBC",
  }
}

export default class Nav extends React.Component<{}, {}> {
  public render() {
    return (
      <div className="flex flex-row pa3 pa3-ns" style={styles.navBar}>
	<Link to={"/"} className="no-underline">
	    <h2 className="f3 lh-title white ma0">Diporto</h2>
	</Link>
	<div style={{flexGrow: 1}}></div>
	<div style={styles.buttonContainer}>
	    <Link
		className="f6 link dim br3 ph3 pv2 mb2 white"
		style={styles.loginButton}
		to="/login">
		Login
	    </Link>
	    <a className="f6 link dim br3 ph3 pv2 mb2 white" style={styles.signUpButton} href="#">Sign Up</a>
	</div>
      </div>
    )
  }
}