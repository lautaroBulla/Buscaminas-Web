<script setup lang="ts">
  import { ref } from 'vue';
  import { onClickOutside } from '@vueuse/core';
  import { useColorMode } from '#imports';
  
  import menuLight from '~/public/img/themes/light/hamburguerLight.png';
  import closeLigh from '~/public/img/themes/light/closeLight.png';
  import trophyLight from '~/public/img/themes/light/trophyLight.png';
  import logoutLight from '~/public/img/themes/light/logoutLight.png';
  import loginLight from '~/public/img/themes/light/loginLight.png';
  import registerLight from '~/public/img/themes/light/registerLight.png';

  import menuDark from '~/public/img/themes/dark/hamburguerDark.png';
  import closeDark from '~/public/img/themes/dark/closeDark.png';
  import trophyDark from '~/public/img/themes/dark/trophyDark.png';
  import logoutDark from '~/public/img/themes/dark/logoutDark.png';
  import loginDark from '~/public/img/themes/dark/loginDark.png';
  import registerDark from '~/public/img/themes/dark/registerDark.png';


  const { user, isAuthReady, logout } = useAuth();

  const colorMode = useColorMode();
  const isDark = computed(() => colorMode.value === 'dark');

  const show = ref(false);
  const dropdownRef = ref(null);

  onClickOutside(dropdownRef, () => {
    show.value = false;
  })

  const logoutLocal = async () => {
    show.value = false;
    await logout();
  }
</script>

<!--
class= generales
       mobile
       desktop
-->

<template>
  <nav class="flex justify-between items-center w-full 
              px-7.5 py-2 border-b-2 border-b-secondary
              md:px-15 md:py-4 md:border-b-4 md:border-b-secondary"
  >

      <div class="hidden 
              md:flex md:justify-start md:gap-x-5 md:w-1/3"
      >
        <ThemeComponent />
        <LanguageSelectorComponent />
      </div>

      <div class="flex
                  md:justify-center md:w-1/3"
      >
        <NuxtLink to="/">
          <p class="title">
            {{ $t('nav.title') }}
          </p>  
        </NuxtLink>
      </div>
  
      <div class="flex items-center 
                  md:justify-end md:w-1/3"
      >
        
        <div v-if="isAuthReady === false">
          <MinePixelReveal 
            :width="'3px'"
            :height="'3px'"
          />
        </div>

        <div v-else-if="user && isAuthReady === true">
          <div class="flex items-center gap-x-3 md:gap-x-5">

            {{ user.username }}

            <div class="flex flex-col items-center relative" ref="dropdownRef">
              <button 
                class="primary flex items-center gap-x-2"
                @click="show = !show"
              >
                  <span class="hidden md:flex">{{ $t('nav.menu') }}</span>
                  <img
                    :src="
                      show
                        ? (isDark ? closeDark : closeLigh)
                        : (isDark ? menuDark : menuLight)
                    "
                    class="w-[15px] h-[15px]"
                  >
              </button>

              <div v-if="show" class="dropdown"> 
                <NuxtLink 
                  to="/ranking"
                  class="optionsDropdown gap-x-2"
                  :title="$t('nav.ranking')"
                  @click="show = false"
                >
                  <img 
                    :src="isDark ? trophyLight : trophyDark"
                    class="w-[12px] h-[12px] md:w-[15px] md:h-[15px]"
                  >
                  <span>Ranking</span>
                </NuxtLink>
                <NuxtLink 
                  class="optionsDropdown gap-x-2 md:whitespace-nowrap leading-4 md:leading-normal"
                  @click="logoutLocal"
                >
                  <img 
                    :src="isDark ? logoutLight : logoutDark"
                    class="w-[12px] h-[12px] md:w-[15px] md:h-[15px]"
                  >
                  <span>{{ $t('nav.logout') }}</span>
                </NuxtLink>
              </div>
            </div>

          </div>
        </div>

        <div v-else>        
          <div class="flex items-center">

            <div class="flex flex-col items-center relative" ref="dropdownRef">
              <button 
                class="primary flex items-center gap-x-2"
                @click="show = !show"
              >
                  {{ $t('nav.menu') }}
                  <img
                    :src="
                      show
                        ? (isDark ? closeDark : closeLigh)
                        : (isDark ? menuDark : menuLight)
                    "
                    class="w-[15px] h-[15px]"
                  >
              </button>

              <div v-if="show" class="dropdown"> 
                <NuxtLink 
                  to="/ranking"
                  class="optionsDropdown gap-x-2"
                  :title="$t('nav.ranking')"
                  @click="show = false"
                >
                  <img 
                    :src="isDark ? trophyLight : trophyDark"
                    class="w-[12px] h-[12px] md:w-[15px] md:h-[15px]"
                  >
                  <span>Ranking</span>
                </NuxtLink>
                <NuxtLink 
                  to="/auth/register"
                  class="optionsDropdown gap-x-2"
                  @click="show = false"
                >
                  <img 
                    :src="isDark ? registerLight : registerDark "
                    class="w-[12px] h-[12px] md:w-[15px] md:h-[15px]"
                  >
                  <span>{{ $t('nav.register') }}</span>
                </NuxtLink>
                <NuxtLink 
                  to="/auth/login"
                  class="optionsDropdown gap-x-2 md:whitespace-nowrap leading-4 md:leading-normal"
                  @click="show = false"
                >
                  <img 
                    :src="isDark ? loginLight : loginDark"
                    class="w-[12px] h-[12px] md:w-[15px] md:h-[15px]"
                  >
                  <span>{{ $t('nav.login') }}</span>
                </NuxtLink>
              </div>
            </div>

          </div>
        </div>

      </div>

  </nav>
</template>