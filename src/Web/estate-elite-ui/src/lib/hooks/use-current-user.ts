import identityService from '@/services/identity-service';
import { useCallback, useEffect } from 'react';
import toast from 'react-hot-toast';
import { useAppDispatch, useAppSelector } from '../hooks';
import { selectIsAuthenticated, updateUser } from '@/redux/slices/auth-slice';

export const useCurrentUser = () => {
  const isAuthenticated = useAppSelector(selectIsAuthenticated);
  const dispatch = useAppDispatch();

  const fetchCurrentUser = useCallback(async () => {
    try {
      const response = await identityService.getCurrentUser();

      if (!response.succeeded || !response.data) {
        toast.error('Xảy ra lỗi trong quá trình lấy thông tin người dùng');
        return;
      }

      dispatch(updateUser(response.data));
      return response.data;
    } catch (error) {
      toast.error('Đã xảy ra lỗi vui lòng thử lại');
      throw error;
    }
  }, [dispatch]);

  useEffect(() => {
    if (isAuthenticated) {
      fetchCurrentUser();
    }
  }, [isAuthenticated, fetchCurrentUser]);

  return { refresh: fetchCurrentUser };
};
