import * as React from 'react';
import { HTMLProps } from 'react';


const styles = {
  navBar: {
      display: "flex",
      flexDirection: "row",
      backgroundColor: "#273CFE",
      paddingTop: "2rem",
      paddingBottom: "2rem"
  } as HTMLProps<HTMLDivElement>,
  buttonContainer: {
      alignSelf: "center"
  } as HTMLProps<HTMLDivElement>,
  title: {
    color: "white",
    margin: 0,
    alignSelf: "center"
  } as HTMLProps<HTMLDivElement>,
  buttons: {
      fontSize: "1.5rem",
      borderRadius: "9999px",
      paddingTop: ".5rem",
      paddingBottom: ".5rem",
      paddingLeft: "1rem",
      paddingRight: "1rem",
      color: "white",
  },
  loginButton: {
      backgroundColor: "#50CCBC",
  },
  signUpButton: {
      color: "#FE92DE",
      borderStyle: "solid",
      borderWidth: ".125rem"
  }
}

export default class Nav extends React.Component<{}, {}> {
  public render() {
    return (
      <div style={styles.navBar}>
	  <h2 style={styles.title}>Diporto</h2>
	  <div style={{flexGrow: 1}}></div>
	  <div style={styles.buttonContainer}>
	      <a style={Object.assign({}, styles.buttons, styles.signUpButton)} href="#">Sign Up</a>
	      <a style={Object.assign({}, styles.buttons, styles.loginButton)} href="#">Login</a>
	  </div>
      </div>
    )
  }
}