import * as React from 'react';
import { RouteComponentProps } from 'react-router-dom';
import LoginForm from './LoginForm';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import * as AuthState from '../store/Auth';

type LoginProps =
  AuthState.AuthState
  & typeof AuthState.actionCreators
  & RouteComponentProps<{}>

class Login extends React.Component<LoginProps, {}> {
  constructor(props) {
    super(props);

    this.loginUser = this.loginUser.bind(this);
  }

  loginUser(creds: Credentials) {
    this.props.loginUser(creds);
  }

  render() {
    return (
      <div className="w-80 self-center pt3">
        {this.props.errorMessage &&
          <div className="flex items-center justify-center pa4 bg-washed-red dark-red br3">
            <svg className="w1" data-icon="info" viewBox="0 0 32 32" style={{fill:"currentcolor"}}>
              <title>info icon</title>
              <path d="M16 0 A16 16 0 0 1 16 32 A16 16 0 0 1 16 0 M19 15 L13 15 L13 26 L19 26 z M16 6 A3 3 0 0 0 16 12 A3 3 0 0 0 16 6"></path>
            </svg>
            <span className="lh-title ml3">{this.props.errorMessage}</span>
          </div>
        }
        <LoginForm
          onLoginPressed={(creds) => this.loginUser(creds)}
        />
      </div>
    )
  }
}


export default connect(
  (state: ApplicationState) => state.auth,
  AuthState.actionCreators
)(Login) as typeof Login;