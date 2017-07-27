import * as React from 'react';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';
import { HTMLProps } from 'react';
import { ApplicationState } from '../../store';
import * as AuthState from '../../store/Auth';
import NavButtons from '../NavButtons';
import ProfileDropdown from '../ProfileDropdown';

const styles = {
  navBar: {
      backgroundColor: "#273CFE",
      flexShrink: 0,
  } as HTMLProps<HTMLDivElement>,
}

type NavProps =
    AuthState.AuthState
    & typeof AuthState.actionCreators

class Nav extends React.Component<NavProps, {}> {
  public render() {
    const { isAuthenticated, user } = this.props;

    return (
      <div className="flex flex-row pa3 pa3-ns" style={styles.navBar}>
        <Link to={"/"} className="no-underline">
            <h2 className="f3 lh-title white ma0">Diporto</h2>
        </Link>
        <div style={{flexGrow: 1}}></div>
        {!isAuthenticated &&
          <NavButtons />
        }
        {isAuthenticated && user &&
          <ProfileDropdown
            user={user}
            onLogout={this.props.logout}/>
        }
      </div>
    )
  }
}

export default connect(
    (state: ApplicationState) => state.auth,
    AuthState.actionCreators
)(Nav) as typeof Nav;