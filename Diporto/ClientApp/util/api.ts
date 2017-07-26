import 'whatwg-fetch';

export class DiportoApi {
  getPlace(id): Promise<Place> {
    const fetchTask = fetch(`/api/places/${id}`)
    .then(response => {
      return response.json() as Promise<Place>
    })

    return fetchTask;
  }
  login(creds: Credentials): Promise<TokenResponse> {
    const fetchTask = fetch('/api/token', {
      method: 'POST',
      headers: {
	'Content-Type': 'application/json'
      },
      body: JSON.stringify({
	UserName: creds.username,
	Password: creds.password,
	GrantType: "access_token"
      })
    })
    .then(response => response.json());

    return fetchTask;
  }
  getMe(token): Promise<User> {
    const config = {
      headers: { 'Authorization' : `Bearer ${token}` }
    }

    const fetchTask = fetch('/api/me', config)
      .then(response => response.json() as Promise<User>)

    return fetchTask;
  }
  signUp(regCreds: UserRegistration) {
    const fetchTask = fetch('/api/register', {
      method: 'POST',
      headers: {
	'Content-Type': 'application/json'
      },
      body: JSON.stringify({
	UserName: regCreds.UserName,
	Password: regCreds.Password,
	ConfirmPassword: regCreds.ConfirmPassword,
	Email: regCreds.Email,
	Name: regCreds.Name
      })
    })
    .then(handleErrors)

    return fetchTask;
  }

}

function handleErrors(response) {
    if (!response.ok) {
	throw Error(response.statusText);
    }
    return response;
}

export default new DiportoApi();