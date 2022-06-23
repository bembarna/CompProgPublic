import React from 'react';

type WrapperTypes = {
  loading: boolean;
};

export const LoadingWrapper: React.FC<WrapperTypes> = ({
  children,
  loading,
}) => {
  if (loading) {
    return <></>;
  } else {
    return <>{children}</>;
  }
};
