
import * as React from 'react';
import TextFieldGroup from '../TextFieldGroup';

declare interface LoginFormProps {
  onLoginPressed: (credentials: Credentials) => void
}

declare interface LoginFormState {
  identifier: string,
  password: string,
  isLoading: boolean
}

class LoginForm extends React.Component<LoginFormProps, LoginFormState> {
  constructor(props) {
    super(props);
    this.state = {
      identifier: '',
      password: '',
      isLoading: false
    };

    this.onSubmit = this.onSubmit.bind(this);
    this.onChange = this.onChange.bind(this);
  }

  onSubmit(e) {
    e.preventDefault();
    this.props.onLoginPressed({username: this.state.identifier, password: this.state.password});
  }

  onChange(e) {
    this.setState({ [e.target.name] : e.target.value });
  }

  render() {
    const { identifier, password, isLoading } = this.state;

    return (
      <form onSubmit={this.onSubmit}>
        <h1>Login</h1>

        <TextFieldGroup
          field="identifier"
          label="Username / Email"
          value={identifier}
          onChange={this.onChange}
          type="text"
        />

        <TextFieldGroup
          field="password"
          label="Password"
          value={password}
          onChange={this.onChange}
          type="password"
        />

        <div><button disabled={isLoading}>Login</button></div>
      </form>
    )
  }
}

export default LoginForm;