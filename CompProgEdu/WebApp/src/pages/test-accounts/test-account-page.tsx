import React from 'react';
import { Form as SUIForm, Divider, Button, Segment } from 'semantic-ui-react';
import { TestAccountService } from '../../swagger';
import { Form } from 'react-final-form';
import { RFFInput, RFFCheckbox } from '../../component/forms';
import { AuthorizedComponent } from '../../component/auth-wrappers';

export const TestAccountPage = () => {
  const getsom = async () => {
    var test = TestAccountService.getById({ id: 1 });
    console.log(test);
  };

  const onSubmit = async (values: any) => {
    const response = await TestAccountService.create({ body: values });

    if (response.errors) {
      return response;
    }
  };

  return (
    <AuthorizedComponent>
      <Divider />
      <Form
        onSubmit={onSubmit}
        render={({ handleSubmit, hasSubmitErrors }) => {
          return (
            <SUIForm onSubmit={handleSubmit} error={hasSubmitErrors}>
              <Segment style={{ width: 500, padding: 50, margin: 20 }}>
                <Button onClick={() => getsom()}>
                  Test get endpoint for auth after log in
                </Button>
                <RFFInput label="Account Number" fieldName="accountNumber" />
                <RFFInput label="Account Name" fieldName="accountName" />
                <RFFInput label="Email Address" fieldName="emailAddress" />
                <RFFCheckbox label="Is Premium?" fieldName="isPremium" toggle />
                <RFFInput
                  fieldName="numberOfPeople"
                  label="Number of People"
                  number
                />
                <div>
                  <Button type="submit">Create</Button>
                </div>
              </Segment>
            </SUIForm>
          );
        }}
      />
    </AuthorizedComponent>
  );
};
