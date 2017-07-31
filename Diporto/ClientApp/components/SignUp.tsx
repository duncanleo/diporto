import * as React from 'react';
import { RouteComponentProps } from 'react-router-dom';
import SignupForm from './SignupForm';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import * as AuthState from '../store/Auth';

type LoginProps =
  AuthState.AuthState
  & typeof AuthState.actionCreators
  & RouteComponentProps<{}>

class SignUp extends React.Component<LoginProps, {}> {
  constructor(props) {
    super(props);

    this.signupUser = this.signupUser.bind(this);
  }

  signupUser(regCreds: UserRegistration) {
    console.log(regCreds);
    this.props.signupUser(regCreds, this.props.loginUser);
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
        <SignupForm
          onSignupPressed={(regCreds) => this.signupUser(regCreds)}
        />
      </div>
    )
  }
}


export default connect(
  (state: ApplicationState) => state.auth,
 AuthState.actionCreators
)(SignUp) as typeof SignUp;