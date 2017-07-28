import * as React from 'react';
import { Link } from 'react-router-dom';
import { HTMLProps } from 'react';

const NavButtons: React.SFC<{}> = (props) => {
  const styles = {
    buttonContainer: {
      alignSelf: "center"
    } as HTMLProps<HTMLDivElement>,
    loginButton: {
      color: "#FE92DE"
    },
    signUpButton: {
      backgroundColor: "#50CCBC"
    }
  }

  return (
    <div style={styles.buttonContainer}>
      <Link
          className="f6 link dim br3 ph3 pv2 mb2 white"
          style={styles.loginButton}
          to="/login">
          Login
      </Link>
      <Link
        className="f6 link dim br3 ph3 pv2 mb2 white"
        style={styles.signUpButton}
        to="/signup">
        Sign Up
      </Link>
    </div>
  )
}

export default NavButtons;