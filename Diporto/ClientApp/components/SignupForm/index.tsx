
import * as React from 'react';
import TextFieldGroup from '../TextFieldGroup';

declare interface SignupFormProps {
  onSignupPressed: (regCredentials: UserRegistration) => void
}

type SignupFormState = UserRegistration

class SignupForm extends React.Component<SignupFormProps, SignupFormState> {
  constructor(props) {
    super(props);
    this.state = {
      UserName: "",
      Password: "",
      ConfirmPassword: "",
      Email: "",
      Name: ""
    };

    this.onSubmit = this.onSubmit.bind(this);
    this.onChange = this.onChange.bind(this);
  }

  onSubmit(e) {
    e.preventDefault();
    this.props.onSignupPressed({
      UserName: this.state.UserName,
      Password: this.state.Password,
      ConfirmPassword: this.state.Password,
      Email: this.state.Email,
      Name: this.state.Name
    })
  }

  onChange(e) {
    this.setState({ [e.target.name] : e.target.value });
  }

  render() {
    const { UserName, Password, ConfirmPassword, Email, Name } = this.state;

    return (
      <form onSubmit={this.onSubmit}>
        <h1>Sign Up</h1>

        <TextFieldGroup
          field="Name"
          label="Name"
          value={Name}
          onChange={this.onChange}
          required={true}
          type="text"
        />

        <TextFieldGroup
          field="UserName"
          label="Username"
          value={UserName}
          onChange={this.onChange}
          required={true}
          type="text"
        />

        <TextFieldGroup
          field="Email"
          label="Email"
          value={Email}
          onChange={this.onChange}
          required={true}
          type="email"
        />

        <TextFieldGroup
          field="Password"
          label="Password"
          value={Password}
          onChange={this.onChange}
          required={true}
          type="password"
        />

        <div><button>Sign Up</button></div>
      </form>
    )
  }
}

export default SignupForm;