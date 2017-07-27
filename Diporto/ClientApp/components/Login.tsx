import * as React from 'react';
import { RouteComponentProps } from 'react-router-dom';
import LoginForm from './LoginForm';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import * as AuthState from '../store/Auth';

type LoginProps = 
  typeof AuthState.actionCreators
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
      <div>
        <LoginForm
          onLoginPressed={(creds) => this.loginUser(creds)}
        />
      </div>
    )
  }
}


export default connect(
  null,
 AuthState.actionCreators
)(Login) as typeof Login;