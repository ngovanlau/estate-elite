import { Filters } from '@/types';

const createQueryKeys = (scope: string) => ({
  all: [scope] as const,
  lists: () => [...createQueryKeys(scope).all, 'list'] as const,
  list: (filters: Filters) =>
    [...createQueryKeys(scope).lists(), JSON.stringify(Object.entries(filters).sort())] as const,
  details: () => [...createQueryKeys(scope).all, 'detail'] as const,
  detail: (id: number) => [...createQueryKeys(scope).details(), id] as const,
});

export const KEYS = {
  users: createQueryKeys('users'),
};
