import * as React from 'react';
import { HTMLProps } from 'react';
import DiportoMap from '../DiportoMap';


const styles = {
  navBar: {
      backgroundColor: "#273CFE",
      padding: "2rem",
  } as HTMLProps<HTMLDivElement>,
  buttonContainer: {
      alignSelf: "center"
  } as HTMLProps<HTMLDivElement>,
  loginButton: {
      backgroundColor: "#50CCBC",
  },
  signUpButton: {
      color: "#FE92DE",
  }
}

export default class Nav extends React.Component<{}, {}> {
  public render() {
    return (
      <div className="flex flex-row pa3" style={styles.navBar}>
	  <h2 className="f3 lh-title white ma0">Diporto</h2>
	  <div style={{flexGrow: 1}}></div>
	  <div style={styles.buttonContainer}>
	      <DiportoMap />
	      <a className="f6 link dim br3 ph3 pv2 mb2 white" style={styles.signUpButton} href="#">Sign Up</a>
	      <a className="f6 link dim br3 ph3 pv2 mb2 white" style={styles.loginButton} href="#">Login</a>
	  </div>
      </div>
    )
  }
}