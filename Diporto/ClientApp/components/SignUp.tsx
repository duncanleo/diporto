import * as React from 'react';
import { RouteComponentProps } from 'react-router-dom';
import SignupForm from './SignupForm';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import * as AuthState from '../store/Auth';

type LoginProps = 
  typeof AuthState.actionCreators
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
      <div>
	<SignupForm
	  onSignupPressed={(regCreds) => this.signupUser(regCreds)}
	/>
      </div>
    )
  }
}


export default connect(
  null,
 AuthState.actionCreators
)(SignUp) as typeof SignUp;