/*
Este composable se encarga de verificar el tamaño de la pantalla del lado del cliente
*/

export const useIsMobile = () => {
  const isMobile = useState('isMobile', () => {
    if (process.client) {
      return window.matchMedia('(max-width: 768px)').matches;
    }
    return false; 
  });

  const updateIsMobile = () => {
    isMobile.value = window.matchMedia('(max-width: 768px)').matches;
  };

  onMounted(() => {
    updateIsMobile();
    window.addEventListener('resize', updateIsMobile);
  });

  onBeforeUnmount(() => {
    window.removeEventListener('resize', updateIsMobile);
  });

  return { isMobile };
};
