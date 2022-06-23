import React from 'react';

type PaginationFooter = {
  totalCount: number;
};

export const PaginationFooter = () => {
  return <>test</>;
};

export const pageSizeOptions = [
  {
    key: 10,
    value: 10,
    text: '10',
  },
  {
    key: 25,
    value: 25,
    text: '25',
  },
  {
    key: 50,
    value: 50,
    text: '50',
  },
];
